// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 分页获取地区多语言列表 GET /api/LocalizationManage/Culture */
export async function getLocalizationManageCulture(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getLocalizationManageCultureParams,
  options?: { [key: string]: any },
) {
  return request<API.PageLocalizationCultureDto>('/api/LocalizationManage/Culture', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 创建地区多语言 POST /api/LocalizationManage/Culture */
export async function postLocalizationManageCulture(
  body: API.CreateLocalizationCultureDto,
  options?: { [key: string]: any },
) {
  return request<API.RLocalizationCultureDto>('/api/LocalizationManage/Culture', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 获取地区多语言详情 GET /api/LocalizationManage/Culture/${param0} */
export async function getLocalizationManageCultureId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getLocalizationManageCultureIdParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.RLocalizationCultureDto>(`/api/LocalizationManage/Culture/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 删除地区多语言 DELETE /api/LocalizationManage/Culture/${param0} */
export async function deleteLocalizationManageCultureId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteLocalizationManageCultureIdParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.R>(`/api/LocalizationManage/Culture/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 修改多语言资源 PUT /api/LocalizationManage/Resource */
export async function putLocalizationManageResource(
  body: API.UpdateLocalizationResourceDto,
  options?: { [key: string]: any },
) {
  return request<API.R>('/api/LocalizationManage/Resource', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 创建多语言资源 POST /api/LocalizationManage/Resource */
export async function postLocalizationManageResource(
  body: API.CreateLocalizationResourceDto,
  options?: { [key: string]: any },
) {
  return request<API.RLocalizationResourceDto>('/api/LocalizationManage/Resource', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 删除多语言资源 DELETE /api/LocalizationManage/Resource/${param0} */
export async function deleteLocalizationManageResourceId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteLocalizationManageResourceIdParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.R>(`/api/LocalizationManage/Resource/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
