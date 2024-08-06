import open3d as o3d  # open3D 모듈 추가, 이름은 o3d로 정의
import numpy as np
import matplotlib.pyplot as plt  # 그래프 그리기 모듈 추가 plt로 정의
from PIL import Image
import os
import sys
import math  # 표준 수학 함수 추가
import cv2  # openCV 모듈 추가
from imageio import imread  # image read 모듈 추가
import numpy as np  # 넘파이(행렬 계산) 추가
from skimage.transform import rescale


# scikit-image (이미지처리 라이브러리) 에서 rescale 추가

# point cloud
def get_index(color):
    ''' Parse a color as a base-256 number and returns the index
    # 색을 256까지로 나눠서 분석, index 반환
    Args:
        color: A 3-tuple in RGB-order where each element \in [0, 255]
        (0, 0, 0) 과 같은 배열로 나눈 것 -> 색
    Returns:
        index: an int containing the indec specified in 'color'
    '''
    return color[0] * 256 * 256 + color[1] * 256 + color[2]
    # (color0, color1, color2)라는 배열 반환식


def dishow(disp):
    plt.imshow(disp)  # disp라는 이미지를 보여줌
    plt.jet()  # Distance to Camera에 나타나는 color bar의 색 출력
    plt.colorbar(label='Distance to Camera')  # 'Distance to Camera'라는 문구 출력
    plt.title('Depth2Disparity image')  # 'Depth2Disparity image' 라는 title 출력
    plt.xlabel('X Pixel')  # 'X Pixel'이라는 문구 출력
    plt.ylabel('Y Pixel')  # 'Y Pixel'이라는 문구 출력
    plt.plot  # 그래프 그리기
    plt.show()  # 이미지 출력


def gen_pcl(color_raw, depth, idx):
    color_raw = cv2.cvtColor(color_raw, cv2.COLOR_BGR2RGB)
    # openCV를 통한 color convert(반전)
    h, w = np.shape(depth)
    d = depth
    da = 2.0 * math.pi / float(w)
    db = 1.0 * math.pi / float(h)
    a = 0.0
    b = -0.5 * math.pi

    points = []
    colors = []
    for i in range(0, h):
        for j in range(0, w):
            r = d[i, j]

            # 색 변환
            c_0 = color_raw[i, j, 0] / 255.0
            c_1 = color_raw[i, j, 1] / 255.0
            c_2 = color_raw[i, j, 2] / 255.0

            # 구면좌표계 변환
            xx = r * math.cos(a) * math.cos(b)
            yy = r * math.sin(a) * math.cos(b)
            zz = r * math.sin(b)

            arr = [xx, yy, zz]
            color = [c_0, c_1, c_2]
            if int(r) != 1:  # r값이 1이 아니라면 -
                points.append(arr)
                colors.append(color)
            a = a + da
        b = b + db

    pcd = o3d.geometry.PointCloud()
    pcd.points = o3d.utility.Vector3dVector(points)
    pcd.colors = o3d.utility.Vector3dVector(colors)
    pcd.transform([[1, 0, 0, 0], [0, -1, 0, 0, ], [0, 0, -1, 0], [0, 0, 0, 1]])
    o3d.io.write_point_cloud('./pcl' + str(idx) + '.ply', pcd)


color_raw = cv2.imread('./2.png')  # RGB 이미지 입력
# image read
f = cv2.flip(color_raw, 1)  # 좌우 반전
cv2.imwrite("flip.png", f)  # 이미지 저장

color_raw = cv2.resize(color_raw, (1024, 512))
# image resizing

depth = imread('./2_1.png', as_gray=True).astype(np.int16)  # depth 입력
depth = rescale(depth, 0.25)
d = depth
dishow(d)

label = imread('2_2.png')  # segmentation 입력
label_s = np.zeros((2048, 4096))
for i in range(2048):
    for j in range(4096):
        label_s[i, j] = get_index(label[i, j])

# zzz=np.unique(label_s)
# for i,id in enumerate(zzz):
#     mask_v=np.zeros((2048,4096,3))
#     mask_idx= label_s==id
#     mask_v[mask_idx,:]=255
#     mask_r = cv2.resize(mask_v,(1024,512))
#     rgb_logical_mask = np.array(mask_r, dtype=bool)
#     d_logical_mask = rgb_logical_mask[:,:,0]
#     #rgb_log_mask = np.repeat(logical_mask,3,axis=0)
#
#     m_rgb=color_raw*rgb_logical_mask
#     m_depth= depth * d_logical_mask
#
#     gen_pcl(m_rgb,m_depth,i)

# TODO MASK
# TODO poly 64 save

# for i in range(512):
#     for j in range(1024):
#         if sum(color_raw[i,j]) == 0:
#             depth[i,j]=0


# d= d/255.0
#
# d = 1/(d+0.001)
# d= (d - d.min()) / (d.max() - d.min())

# d= 1/(d+0.0001)

# d= d*255
# cv2.imshow('dd',d)
# cv2.waitKey(0)
# d=(1/(d+0.1))
# d=(d - d.min()) / (d.max() - d.min())


color_raw = cv2.cvtColor(color_raw, cv2.COLOR_BGR2RGB)
h, w = np.shape(depth)
d = depth
da = 2.0 * math.pi / float(w)
db = 1.0 * math.pi / float(h)
a = 0.0
b = -0.5 * math.pi

points = []
colors = []
for i in range(0, h):
    for j in range(0, w):
        r = d[i, j]

        c_0 = color_raw[i, j, 0] / 255.0
        c_1 = color_raw[i, j, 1] / 255.0
        c_2 = color_raw[i, j, 2] / 255.0

        xx = r * math.cos(a) * math.cos(b)
        yy = r * math.sin(a) * math.cos(b)
        zz = r * math.sin(b)

        arr = [xx, yy, zz]
        color = [c_0, c_1, c_2]
        if int(r) != 1:
            points.append(arr)
            colors.append(color)
        a = a + da
    b = b + db

pcd = o3d.geometry.PointCloud()
pcd.points = o3d.utility.Vector3dVector(points)
pcd.colors = o3d.utility.Vector3dVector(colors)
pcd.transform([[1, 0, 0, 0], [0, -1, 0, 0, ], [0, 0, -1, 0], [0, 0, 0, 1]])
o3d.io.write_point_cloud('./pcl_V.ply', pcd)
