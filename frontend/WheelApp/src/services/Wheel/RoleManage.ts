// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 创建角色 POST /api/RoleManage */
export async function postRoleManage(body: API.CreateRoleDto, options?: { [key: string]: any }) {
  return request<API.R>('/api/RoleManage', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 删除角色 DELETE /api/RoleManage */
export async function deleteRoleManage(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteRoleManageParams,
  options?: { [key: string]: any },
) {
  return request<API.R>('/api/RoleManage', {
    method: 'DELETE',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 角色分页查询 GET /api/RoleManage/Page */
export async function getRoleManagePage(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getRoleManagePageParams,
  options?: { [key: string]: any },
) {
  return request<API.PageRoleDto>('/api/RoleManage/Page', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}
