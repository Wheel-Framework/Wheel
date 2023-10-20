// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 分页查询列表 GET /api/File */
export async function getFile(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getFileParams,
  options?: { [key: string]: any },
) {
  return request<API.PageFileStorageDto>('/api/File', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 上传文件 POST /api/File */
export async function postFile(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.postFileParams,
  body: {},
  Files?: File[],
  options?: { [key: string]: any },
) {
  const formData = new FormData();

  if (Files) {
    Files.forEach((f) => formData.append('Files', f || ''));
  }

  Object.keys(body).forEach((ele) => {
    const item = (body as any)[ele];

    if (item !== undefined && item !== null) {
      if (typeof item === 'object' && !(item instanceof File)) {
        if (item instanceof Array) {
          item.forEach((f) => formData.append(ele, f || ''));
        } else {
          formData.append(ele, JSON.stringify(item));
        }
      } else {
        formData.append(ele, item);
      }
    }
  });

  return request<API.RListFileStorageDto>('/api/File', {
    method: 'POST',
    params: {
      ...params,
    },
    data: formData,
    requestType: 'form',
    ...(options || {}),
  });
}

/** 下载文件 GET /api/File/${param0} */
export async function getFileId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getFileIdParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/File/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}
