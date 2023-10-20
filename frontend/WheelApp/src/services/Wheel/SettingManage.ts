// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 获取所有设置 GET /api/SettingManage */
export async function getSettingManage(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getSettingManageParams,
  options?: { [key: string]: any },
) {
  return request<API.RListSettingGroupDto>('/api/SettingManage', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 更新设置 PUT /api/SettingManage/${param0} */
export async function putSettingManageSettingScope(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putSettingManageSettingScopeParams,
  body: API.SettingGroupDto,
  options?: { [key: string]: any },
) {
  const { settingScope: param0, ...queryParams } = params;
  return request<API.R>(`/api/SettingManage/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}
