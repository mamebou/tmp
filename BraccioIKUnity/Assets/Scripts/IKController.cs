using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra.Single;

public class IKController : MonoBehaviour
{
    //private GameObject[] joint = new GameObject[6];
    public ArticulationBody[] joint = new ArticulationBody[6];
    private float[] angle = new float[6];
    private Vector3[] dim = new Vector3[6]; // ローカル 座標軸 の ワールド 座標 
    private Vector3[] point = new Vector3[7]; // 回転軸 の 向き 
    private Vector3[] axis = new Vector3[6]; // 各 軸 の 親 基準 ローカル 回転 クオータニオン 
    private Quaternion[] rotation = new Quaternion[6]; // 各 軸 の ワールド 回転 クオータニオン 
    private Quaternion[] wRotation = new Quaternion[6];
    private Vector3 pos; // 目標 位置 
    private Vector3 rot; // 目標 姿勢
    private float lambda = 0.1f;
    public GameObject target;
    private float[] prevAngle = new float[6];
    private float[] minAngle = new float[6];
    private float[] maxAngle = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        //  for (int i = 0; i < joint.Length; i ++) { 
        //     joint[i] = GameObject.Find("Joint_" + i.ToString());
        // }

        // initial value
        dim[0] = new Vector3(0f, 0.06584513f, -0.0643453f);
        dim[1] = new Vector3(-3.109574e-05f, 0.2422461f, -0.005073354f);
        dim[2] = new Vector3(3.109574e-05f, 0.1733774f + 0.04468203f, 0.04262526f - 0.04507105f);
        dim[3] = new Vector3(0f, 0.03749204f, -0.03853976f);
        dim[4] = new Vector3(0f, 0.045f, -0.047f);
        dim[5] = new Vector3(0f, 0f, -0.13f);
        // 各 回転軸 の 方向 
        axis[0] = new Vector3(0f, 1f, 0f);
        axis[1] = new Vector3(0f, 0f, 1f);
        axis[2] = new Vector3(0f, 0f, 1f);
        axis[3] = new Vector3(0f, 0f, 1f);
        axis[4] = new Vector3(0f, 1f, 0f);
        axis[5] = new Vector3(0f, 0f, 1f);
        // イニシャル 姿勢 での 回転角 
        angle[0] = 0f;
        angle[1] = 30f;
        angle[2] = -60f;
        angle[3] = 30f;
        angle[4] = 0f;
        angle[5] = 0f;

        //set limit value
        for (int i = 0; i < joint.Length; i++)
        {
            minAngle[i] = -180f;
            maxAngle[i] = 180f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        pos = target.transform.position;
        rot = target.transform.rotation.eulerAngles;

        CalcIK();
    }

    void CalcIK()
    {
        int count = 0;
        bool outOfLimnit = false;

        for (int i = 0; i < 100; i++)
        {
            count = i;
            ForwardKinematics();
            var err = CalcErr();
            float err_norm = (float)err.L2Norm(); //隔たり の 絶対値
            if (err_norm < 1E-3) // 繰り返し を 抜ける 
            {
                for (int ii = 0; ii < joint.Length; ii++)
                {
                    if (angle[ii] < minAngle[ii] || angle[ii] > maxAngle[ii])
                    {
                        outOfLimnit = true;
                        break;
                    }
                }
                break;
            }
            var J = CalcJacobian(); // ヤコビ 行列 を 求める 
            // 角度 を 修正 
            var dAngle = lambda * J.PseudoInverse() * err;
            for (int ii = 0; ii < joint.Length; ii++)
            {
                angle[ii] += dAngle[ii, 0] * Mathf.Rad2Deg;
            }

        }

        if (count == 99 || outOfLimnit)
        {
            Debug.Log(count);
            for (int i = 0; i < joint.Length; i++)
            {
                angle[i] = prevAngle[i];
            }
        }
        else
        {
            for (int i = 0; i < joint.Length; i++)
            {
                if(i != 3){
                    var drive = joint[i].xDrive;
                    drive.target = angle[i];
                    joint[i].xDrive = drive;
                    // rotation[i] = Quaternion.AngleAxis(angle[i], axis[i]);
                    // joint[i].transform.localRotation = rotation[i];
                    prevAngle[i] = angle[i];
                }
                else{
                    var drive = joint[i].xDrive;
                    drive.target = 50f;
                    joint[i].xDrive = drive;
                    prevAngle[i] = angle[i];
                }
            }
        }

    }

    void ForwardKinematics()
    {
        point[0] = new Vector3(0f, 0.08605486f, 0f);
        wRotation[0] = Quaternion.AngleAxis(angle[0], axis[0]);
        for (int i = 1; i < joint.Length; i++)
        {
            point[i] = wRotation[i - 1] * dim[i - 1] + point[i - 1];
            rotation[i] = Quaternion.AngleAxis(angle[i], axis[i]);
            wRotation[i] = wRotation[i - 1] * rotation[i];
        }
        point[joint.Length] = wRotation[joint.Length - 1] * dim[joint.Length - 1] + point[joint.Length - 1];
    }

    DenseMatrix CalcErr()
    {
        // 位置 誤差 
        Vector3 perr = pos - point[6];
        // 姿勢 誤差 
        Quaternion rerr = Quaternion.Euler(rot) * Quaternion.Inverse(wRotation[5]);
        // xyz 周り の 回転 に 変換 
        Vector3 rerrVal = new Vector3(rerr.eulerAngles.x, rerr.eulerAngles.y, rerr.eulerAngles.z);
        if (rerrVal.x > 180f) rerrVal.x -= 360f;
        if (rerrVal.y > 180f) rerrVal.y -= 360f;
        if (rerrVal.z > 180f) rerrVal.z -= 360f;
        var err = DenseMatrix.OfArray(new float[,]{
            { perr.x },
            { perr.y },
            { perr.z },
            { rerrVal.x * Mathf.Deg2Rad},
            { rerrVal.y * Mathf.Deg2Rad},
            { rerrVal.z * Mathf.Deg2Rad}
        });

        return err;
    }

    DenseMatrix CalcJacobian()
    {
        Vector3 w0 = wRotation[0] * axis[0];
        Vector3 w1 = wRotation[1] * axis[1];
        Vector3 w2 = wRotation[2] * axis[2];
        Vector3 w3 = wRotation[3] * axis[3];
        Vector3 w4 = wRotation[4] * axis[4];
        Vector3 w5 = wRotation[5] * axis[5];
        Vector3 p0 = Vector3.Cross(w0, point[6] - point[0]);
        Vector3 p1 = Vector3.Cross(w1, point[6] - point[1]);
        Vector3 p2 = Vector3.Cross(w2, point[6] - point[2]);
        Vector3 p3 = Vector3.Cross(w3, point[6] - point[3]);
        Vector3 p4 = Vector3.Cross(w4, point[6] - point[4]);
        Vector3 p5 = Vector3.Cross(w5, point[6] - point[5]);
        var J = DenseMatrix.OfArray(new float[,]{
            { p0.x, p1.x, p2.x, p3.x, p4.x, p5.x },
            { p0.y, p1.y, p2.y, p3.y, p4.y, p5.y },
            { p0.z, p1.z, p2.z, p3.z, p4.z, p5.z },
            { w0.x, w1.x, w2.x, w3.x, w4.x, w5.x },
            { w0.y, w1.y, w2.y, w3.y, w4.y, w5.y },
            { w0.z, w1.z, w2.z, w3.z, w4.z, w5.z }
        });

        return J;
    }
}


