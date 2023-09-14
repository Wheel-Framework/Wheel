// @ts-ignore
/* eslint-disable */
import { request } from '@umijs/max'

/** 此处后端没有提供注释 GET /api/identity/confirmEmail */
export async function MapIdentityApiApiIdentityConfirmEmail(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.confirmEmailParams
    ,
  options ?: {[key: string]: any}
) {
  return request<any>('/api/identity/confirmEmail', {
  method: 'GET',
    params: {


        ...params,},
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/forgotPassword */
export async function postIdentityForgotPassword(body: API.ForgotPasswordRequest,
  options ?: {[key: string]: any}
) {
  return request<any>('/api/identity/forgotPassword', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/login */
export async function postIdentityLogin(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.postIdentityLoginParams
    ,body: API.LoginRequest,
  options ?: {[key: string]: any}
) {
  return request<API.AccessTokenResponse>('/api/identity/login', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    params: {

        ...params,},
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/manage/2fa */
export async function postIdentityManage2fa(body: API.TwoFactorRequest,
  options ?: {[key: string]: any}
) {
  return request<API.TwoFactorResponse>('/api/identity/manage/2fa', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/identity/manage/info */
export async function getIdentityManageInfo(
  options ?: {[key: string]: any}
) {
  return request<API.InfoResponse>('/api/identity/manage/info', {
  method: 'GET',
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/manage/info */
export async function postIdentityManageInfo(body: API.InfoRequest,
  options ?: {[key: string]: any}
) {
  return request<API.InfoResponse>('/api/identity/manage/info', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/refresh */
export async function postIdentityRefresh(body: API.RefreshRequest,
  options ?: {[key: string]: any}
) {
  return request<API.AccessTokenResponse>('/api/identity/refresh', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/register */
export async function postIdentityRegister(body: API.RegisterRequest,
  options ?: {[key: string]: any}
) {
  return request<any>('/api/identity/register', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/resendConfirmationEmail */
export async function postIdentityResendConfirmationEmail(body: API.ResendEmailRequest,
  options ?: {[key: string]: any}
) {
  return request<any>('/api/identity/resendConfirmationEmail', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/identity/resetPassword */
export async function postIdentityResetPassword(body: API.ResetPasswordRequest,
  options ?: {[key: string]: any}
) {
  return request<any>('/api/identity/resetPassword', {
  method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

