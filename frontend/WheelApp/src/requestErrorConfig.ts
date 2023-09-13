import type { RequestOptions } from '@@/plugin-request/request';
import type { RequestConfig } from '@umijs/max';
import { message, notification } from 'antd';
import { getAccessToken } from './WheelStore';
import Cookies from 'js-cookie';

const isDev = process.env.NODE_ENV === 'development';

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
        // Axios 的错误
        // 请求成功发出且服务器也响应了状态码，但状态代码超出了 2xx 的范围
        message.error(`Response status:${error.response.status}`);
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
