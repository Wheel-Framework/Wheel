// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max';

/** 获取枚举键值对数据 GET /api/SupportData/Enums/${param0} */
export async function getSupportDataEnumsType(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getSupportDataEnumsTypeParams,
  options?: { [key: string]: any },
) {
  const { type: param0, ...queryParams } = params;
  return request<API.RListLabelValueDto>(`/api/SupportData/Enums/${param0}`, {
    method: 'GET',
    params: {
      ...queryParams,
    },
    ...(options || {}),
  });
}
