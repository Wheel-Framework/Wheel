// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 获取当前用户菜单信息 GET /api/Current/menu */
export async function getCurrentMenu(options?: { [key: string]: any }) {
  return request<API.RListAntdMenuDto>('/api/Current/menu', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 获取当前用户信息 GET /api/Current/user */
export async function getCurrentUser(options?: { [key: string]: any }) {
  return request<API.RICurrentUser>('/api/Current/user', {
    method: 'GET',
    ...(options || {}),
  });
}
