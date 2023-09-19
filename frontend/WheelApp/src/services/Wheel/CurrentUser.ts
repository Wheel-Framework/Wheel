// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 此处后端没有提供注释 GET /api/CurrentUser */
export async function getCurrentUser(options?: { [key: string]: any }) {
  return request<API.RICurrentUser>('/api/CurrentUser', {
    method: 'GET',
    ...(options || {}),
  });
}
