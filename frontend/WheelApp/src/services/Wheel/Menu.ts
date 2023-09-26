// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 查询菜单列表 GET /api/Menu */
export async function getMenu(options?: { [key: string]: any }) {
  return request<API.RListMenuDto>('/api/Menu', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 新增菜单 POST /api/Menu */
export async function postMenu(body: API.CreateOrUpdateMenuDto, options?: { [key: string]: any }) {
  return request<API.R>('/api/Menu', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 获取单个菜单详情 GET /api/Menu/${param0} */
export async function getMenuId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getMenuIdParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.RMenuDto>(`/api/Menu/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 修改菜单 PUT /api/Menu/${param0} */
export async function putMenuId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putMenuIdParams,
  body: API.CreateOrUpdateMenuDto,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.R>(`/api/Menu/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 删除菜单 DELETE /api/Menu/${param0} */
export async function deleteMenuId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteMenuIdParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.R>(`/api/Menu/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 获取角色菜单列表 GET /api/Menu/role/${param0} */
export async function getMenuRoleRoleId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getMenuRoleRoleIdParams,
  options?: { [key: string]: any },
) {
  const { roleId: param0, ...queryParams } = params;
  return request<API.RListMenuDto>(`/api/Menu/role/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 修改角色菜单 PUT /api/Menu/role/${param0} */
export async function putMenuRoleRoleId(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putMenuRoleRoleIdParams,
  body: API.UpdateRoleMenuDto,
  options?: { [key: string]: any },
) {
  const { roleId: param0, ...queryParams } = params;
  return request<API.R>(`/api/Menu/role/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}
