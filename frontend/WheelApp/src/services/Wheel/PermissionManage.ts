// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 获取所有权限 GET /api/PermissionManage */
export async function getPermissionManage(options?: { [key: string]: any }) {
  return request<API.RListGetAllPermissionDto>('/api/PermissionManage', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 修改权限 PUT /api/PermissionManage */
export async function putPermissionManage(
  body: API.UpdatePermissionDto,
  options?: { [key: string]: any },
) {
  return request<API.R>('/api/PermissionManage', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 获取指定角色权限 GET /api/PermissionManage/${param0} */
export async function getPermissionManageRole(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getPermissionManageRoleParams,
  options?: { [key: string]: any },
) {
  const { role: param0, ...queryParams } = params;
  return request<API.RListGetAllPermissionDto>(`/api/PermissionManage/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}
