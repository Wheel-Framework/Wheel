// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 用户分页查询 GET /api/UserManage */
export async function getUserManage(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getUserManageParams,
  options?: { [key: string]: any },
) {
  return request<API.PageUserDto>('/api/UserManage', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 创建用户 POST /api/UserManage */
export async function postUserManage(body: API.CreateUserDto, options?: { [key: string]: any }) {
  return request<API.R>('/api/UserManage', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}
