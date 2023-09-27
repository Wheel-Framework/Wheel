import type { RequestOptions } from '@@/plugin-request/request';
import type { RequestConfig } from '@umijs/max';
import { message, notification } from 'antd';
import { getAccessToken } from './WheelStore';
import Cookies from 'js-cookie';
import { getLocale, history } from '@umijs/max';

const isDev = process.env.NODE_ENV === 'development';
const loginPath = '/user/login';

// 错误处理方案： 错误类型
enum ErrorShowType {
  SILENT = 0,
  WARN_MESSAGE = 1,
  ERROR_MESSAGE = 2,
  NOTIFICATION = 3,
  REDIRECT = 9,
}
// 与后端约定的响应数据格式
interface ResponseStructure {
  code: string;
  data: any;
  message?: string;
}



/**
 * 异常处理程序
    200: '服务器成功返回请求的数据。',
    201: '新建或修改数据成功。',
    202: '一个请求已经进入后台排队（异步任务）。',
    204: '删除数据成功。',
    400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
    401: '用户没有权限（令牌、用户名、密码错误）。',
    403: '用户得到授权，但是访问是被禁止的。',
    404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
    405: '请求方法不被允许。',
    406: '请求的格式不可得。',
    410: '请求的资源被永久删除，且不会再得到的。',
    422: '当创建一个对象时，发生一个验证错误。',
    500: '服务器发生错误，请检查服务器。',
    502: '网关错误。',
    503: '服务不可用，服务器暂时过载或维护。',
    504: '网关超时。',
 * @see https://beta-pro.ant.design/docs/request-cn
 */
    const codeMessage: Record<number, string> = {
      200: '服务器成功返回请求的数据。',
      201: '新建或修改数据成功。',
      202: '一个请求已经进入后台排队（异步任务）。',
      204: '删除数据成功。',
      400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
      401: '用户没有权限（令牌、用户名、密码错误）。',
      403: '用户得到授权，但是访问是被禁止的。',
      404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
      405: '请求方法不被允许。',
      406: '请求的格式不可得。',
      410: '请求的资源被永久删除，且不会再得到的。',
      422: '当创建一个对象时，发生一个验证错误。',
      500: '服务器发生错误，请检查服务器。',
      502: '网关错误。',
      503: '服务不可用，服务器暂时过载或维护。',
      504: '网关超时。',
    };
/**
 * @name 错误处理
 * pro 自带的错误处理， 可以在这里做自己的改动
 * @doc https://umijs.org/docs/max/request#配置
 */
export const errorConfig: RequestConfig = {
  // 错误处理： umi@3 的错误处理方案。
  errorConfig: {
    // 错误抛出
    errorThrower: (res) => {
      const { data, code, message } =
        res as unknown as ResponseStructure;
      if (code !== "0") {
        throw res; // 抛出自制的错误
      }
    },
    // 错误接收及处理
    errorHandler: (error: any, opts: any) => {
      if (opts?.skipErrorHandler) throw error;
      // 我们的 errorThrower 抛出的错误。
      if (error.response) {

        console.log(error.response)
        const { response, code, message, config } = error;

        const errorText = codeMessage[response.status] || message;

        const { status, url, data } = response;

        const responseError = data as ResponseStructure | undefined
        if(status === 401){
          message.error('没有权限');
          history.push(loginPath);
        }
        else if(status === 400 && responseError){
          notification.error({
            message: `请求错误`,
            description: responseError.message,
          });
        }
        else if (responseError?.message) {
          notification.error({
            message: `请求错误`,
            description: responseError?.message,
          });
        } else {
          notification.error({
            message: `请求错误  ${status}: ${config.url}`,
            description: errorText,
          });
        }
      } else if (error.request) {
        // 请求已经成功发起，但没有收到响应
        // \`error.request\` 在浏览器中是 XMLHttpRequest 的实例，
        // 而在node.js中是 http.ClientRequest 的实例
        message.error('None response! Please retry.');
      } else {
        // 发送请求时出了点问题
        message.error('Request error, please retry.');
      }
    },
  },

  // 请求拦截器
  requestInterceptors: [
    (config: RequestOptions) => {
      // 拦截请求配置，进行个性化处理。
      const token = getAccessToken() || undefined;
      if(token){
        config.headers = Object.assign(config.headers, {
          Authorization: 'Bearer ' + token
        })
      }
      var XSRFTOKEN = Cookies.get("XSRF-TOKEN")
      if(XSRFTOKEN){
        config.headers = Object.assign(config.headers, {
          'RequestVerificationToken': XSRFTOKEN,
        })
      }
      config.headers = Object.assign(config.headers, {
        'Accept-Language': getLocale()
      })
      let url = config?.url;
      if(!isDev){
        if (/^\/api/.test(url)) {
          url = `${API_URL}${url}`;
        }

      }
      return { ...config, url };
    },
  ],

  // 响应拦截器
  responseInterceptors: [
    (response) => {
      // 拦截响应数据，进行个性化处理
      // const { data } = response as unknown as ResponseStructure;

      // if (data?.success === false) {
      //   message.error('请求失败！');
      // }
      return response;
    },
  ],
};
