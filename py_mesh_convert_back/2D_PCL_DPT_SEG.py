import cv2 # OpenCV 모듈 추가
import open3d as o3d # open3d 모듈 추가, 이름을 o3d로 정의
import open3d.cpu.pybind.io # open3d pybind 추가 ---- 지금까지 3차원 데이터 다루기 용이한 모듈
import numpy as np # numpy 모듈 추가, 이름을 np로 정의 (배열 연산 모듈)
import matplotlib.pyplot as plt # matplotlib 모듈 추가, 이름을 plt로 정의 (그래프 만들기 모듈)
from imageio import imread # imageio 모듈에서 imread를 뽑아서 써야할 때(imageio.imread) 그냥 imread만 사용할 수 있게 해줌.
from DPT import DPT # Depth를 딥러닝 네트워크로 예측하는 것
import imageio

from skimage.filters import gaussian, sobel # sobel : 이미지에서 edge를 찾는 필터

from scipy.interpolate import griddata

import time


################################################################################################
##파일 실행하시면 depth 등등을 보여주는 팝업창이 4개가 잇달아 뜹니다. 그거 뜨는대로 꺼주셔야 코드 계속 진행됩니다##
################################################################################################

def depth_exception(x,y,z,depth): # depth_exception 정의
  vert = [] # 행렬 선언
  vert.append(x)
  vert.append(y)
  vert.append(z)
  depth_except = 0
  for k in range(3): # 3 크기의 range에서 k를 반복
    if np.round(depth[vert[k]], 1) < 0.001:  #numpy.round -> 소수점 한자리 반올림. vert 행렬 k 값이 0.001보다 작을 때
      return 1
    else :
      continue
  return 0

def dishowdepth(disp): # dishow 정의, 함수 그리기.
  plt.imshow(disp)
  plt.jet()
  plt.colorbar(label='Distance to Camera')
  plt.title('Depth image') # 그래프 이름
  plt.xlabel('X Pixel') # X축
  plt.ylabel('Y Pixel') # Y축
  plt.plot
  plt.show()

def dishowedge(disp):
  plt.imshow(disp)
  plt.jet()
  plt.title('Edge image')  # 그래프 이름
  plt.xlabel('X Pixel')  # X축
  plt.ylabel('Y Pixel')  # Y축
  plt.plot
  plt.show()

def dishowdepth2Disparity(disp): # dishow 정의, 함수 그리기.
  plt.imshow(disp)
  plt.jet()
  plt.colorbar(label='Distance to Camera')
  plt.title('Depth2Disparity image') # 그래프 이름
  plt.xlabel('X Pixel') # X축
  plt.ylabel('Y Pixel') # Y축
  plt.plot
  plt.show()

def projection(rgb, dp, mask):
  H, W, _ = rgb.shape
  x = np.linspace(1, W, W)
  y = np.linspace(1, H, H)
  z = np.linspace(1, 1, H * W)

  X, Y = np.meshgrid(x, y) # 격자 생성

  X = X[mask]
  Y = Y[mask]
  dp = dp[mask]
  # Y = Y = np.flip(Y, axis=0)
  f = (H ** 2 + W ** 2) ** 0.5
  c_px = W / 2
  c_py = H / 2
  X = (X - c_px) * dp / f
  Y = (Y - c_py) * dp / f

  #X = (X ) * dp / f
  #Y = (Y ) * dp / f
  points = np.stack([X, Y, dp], -1).reshape(-1, 3)
  return points


def disp_sharpeneing(disparity):
  H,W= disparity.shape
  xs, ys = np.meshgrid(np.arange(H), np.arange(W))
  edges = sobel(disparity) > 20   #이게 EDGE 추출 강도 조정하는 패러미터. 10, 20 ,6 뭐 이렇게 크게조정. 숫자 클수록 기준올라가서 더 강한 엣지 추출
  #dishowedge(edges) #edge 팝업.안뜨게해놓음
  disparity[edges] = 0
  mask = disparity > 0
  try:
    disparity = griddata(np.stack([ys[mask].ravel(), xs[mask].ravel()], 1),
                         disparity[mask].ravel(), np.stack([ys.ravel(), xs.ravel()], 1),
                         method='nearest').reshape(H,W)
  except (ValueError, IndexError) as e:
    pass  # just return disparity
  return disparity, mask




# 메인 함수

start_time2 = time.time() #시간측정용 TOTAL START
start_time_est = time.time() #시간측정용 EST START

###############################################################
##사용할 2D 이미지 경로 설정
path='./For2D/input/'
name='image01'  #인풋 이미지 파일 이름
###################################################인풋되는 이미지의 경로

# label = np.load(path+name+'.npy')
# unique=np.unique(label)


rgb= imread(path+name+'.jpg')

#rgb = cv2.resize(rgb, (1920,1080))  # 1차적으로 1920*1080으로 비율조정
h,w,c= rgb.shape
#print(type(h),type(w),type(c))
rgb = cv2.resize(rgb, (w//4, h//4))
#rgb= cv2.cvtColor(rgb,cv2.COLOR_BGR2RGB)
cv2.imwrite(path+name+'_resize.jpg',rgb)
rgb= imageio.core.util.Array(rgb)
rgb= rgb/255.0


#github 레포지토리의 hubconf.py의 기반
#수정된 DPT.py 파일 필요

#dp = DPT(path+name+'.jpg', "DPT_Large") # 네트워크
#dp = DPT(path+name+'.jpg', "DPT_Hybrid") # 네트워크
#dp = DPT(path+name+'.jpg', "MiDaS_small") # 네트워크
dp = DPT(path+name+'.jpg', "DPT_BEiT_L_512") # 네트워크   #이게 제일 성능 괜찮음
#dp = DPT(path+name+'.jpg', "DPT_BEiT_L_384") # 네트워크

dp = (dp-dp.min())/(dp.max()-dp.min())

dp = 1/(dp+0.1)

dp = (dp-dp.min())/(dp.max()-dp.min())

dp = dp*255

#dishowdepth(dp) # edge가 적용되지 않은 Depth 팝업

dp, mask = disp_sharpeneing(dp) #edge image 표시

#dishowdepth2Disparity(dp) #depth2Dis 팝업

# DPT Version

dp = (dp-dp.min())/(dp.max()-dp.min()) # DP 정규화
dp = dp + 0.3

#dishowdepth2Disparity(dp) #deph2dis 팝업

points = projection(rgb, dp, mask)


rgb = rgb[mask,:]
colors = rgb.reshape(-1,3)




pcd = o3d.geometry.PointCloud() # open3d를 통해 포인트 클라우드 추출

pcd.points = o3d.utility.Vector3dVector(points) # projection 함수로 만들어진 points를 pcd에 투영

pcd.colors = o3d.utility.Vector3dVector(colors) # 메쉬에 색상 부여

pcd.transform([[1, 0, 0, 0], [0, -1, 0, 0], [0, 0, -1, 0], [0, 0, 0, 1]])

pcd.estimate_normals()

end_time_est = time.time() #시간측정용 EST END
print(f"Estimation Elapsed Time: {end_time_est-start_time_est:.4f} seconds") #시간출력 EST

distances = pcd.compute_nearest_neighbor_distance()
avg_dist = np.mean(distances) # 포인트 클라우드 점 간의 평균 거리

start_time = time.time() #시간측정용 PIVOT START

##
radius_parameter = 5  ######여기서 볼 피봇팅 래디우스 패러미터조정함. 3,4,5 늘려가면서 가능###
##

radius = radius_parameter * avg_dist
bp_mesh = o3d.geometry.TriangleMesh.create_from_point_cloud_ball_pivoting(pcd, o3d.utility.DoubleVector([radius, radius * 2]))
bp_mesh.triangle_normals = o3d.utility.Vector3dVector([])


######################################################################################
#o3d.io.write_triangle_mesh(f'./For2D/output/test3_output_obj_{name}_rad_{radius}_radpara_{radius_parameter}_edge_4_DPT_BEiT_L_512.obj', bp_mesh) # mesh를 obj 형태로 변환하여 저장
o3d.io.write_triangle_mesh(f'./For2D/output/image01_mesh.obj', bp_mesh) # mesh를 obj 형태로 변환하여 저장
#################################################################################### 아웃풋 되는 메시

end_time = time.time() #시간측정용 PIVOT END
print(f"Radius: {radius}, Ball Pivoting Elapsed Time: {end_time-start_time:.4f} seconds") #시간출력 PIVOT
print("Radius Parameter:",radius_parameter) #볼피봇팅 래디우스 패러미터 출력

#o3d.io.write_point_cloud(f'./For2D/output/test3_output_ply_{name}_rad_{radius}_radpara_{radius_parameter}_edge_4_DPT_BEiT_L_512.ply', pcd) # pointcloud를 ply 형태로 변환하여 저장

end_time2 = time.time() #시간측정용 TOTAL END
print(f"Radius: {radius}, Total Elapsed Time: {end_time2-start_time2:.4f} seconds") #시간출력 TOTAL

