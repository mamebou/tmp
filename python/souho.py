import math 
import numpy as np

# IMUの初期設定（データはIMUから取得する必要があります）
gyroscope_data = np.array([0.0, 0.0, 0.0])  # ジャイロスコープのデータ (rad/s)
acceleration_data = np.array([0.0, 0.0, 9.81])  # 加速度計のデータ (m/s^2)

# フィルタの初期設定
alpha = 0.98  # ジャイロスコープの重み
dt = 0.01  # サンプリング間隔（秒）

# 姿勢の初期化（クォータニオンを使用）
q = np.array([1.0, 0.0, 0.0, 0.0])  # 初期クォータニオン [w, x, y, z]

# クォータニオンの正規化
def normalize_quaternion(q):
    norm = np.linalg.norm(q)
    return q / norm

# クォータニオンからオイラー角を計算
def quaternion_to_euler(q):
    roll = math.atan2(2 * (q[0]*q[1] + q[2]*q[3]), 1 - 2 * (q[1]**2 + q[2]**2))
    pitch = math.asin(2 * (q[0]*q[2] - q[3]*q[1]))
    yaw = math.atan2(2 * (q[0]*q[3] + q[1]*q[2]), 1 - 2 * (q[2]**2 + q[3]**2))
    return roll, pitch, yaw

# 姿勢推定ループ
for i in range(1000):  # 1000回のサンプルを処理する例
    # ジャイロスコープデータから角速度変化を計算
    gyro_roll, gyro_pitch, gyro_yaw = gyroscope_data * dt

    # クォータニオンを更新
    q += np.array([0.5 * gyro_roll, 0.5 * gyro_pitch, 0.5 * gyro_yaw, 0.0]) * q
    q = normalize_quaternion(q)

    # 加速度計データからロールとピッチを計算
    accel_roll, accel_pitch = np.arctan2(acceleration_data[1], acceleration_data[2]), -np.arctan2(acceleration_data[0], np.sqrt(acceleration_data[1]**2 + acceleration_data[2]**2))

    # クォータニオンと加速度計データから得たロールとピッチを統合
    q_accel = np.array([math.cos(0.5*accel_pitch), math.sin(0.5*accel_pitch), math.cos(0.5*accel_roll), math.sin(0.5*accel_roll)])
    q = q_accel * q

    # クォータニオンを正規化
    q = normalize_quaternion(q)

    # クォータニオンからオイラー角を計算
    roll, pitch, yaw = quaternion_to_euler(q)

    # 姿勢を出力
    print("Roll: {:.2f} degrees, Pitch: {:.2f} degrees, Yaw: {:.2f} degrees".format(
        math.degrees(roll), math.degrees(pitch), math.degrees(yaw)))
