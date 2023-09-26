import Footer from '@/components/Footer';
import { Question, SelectLang } from '@/components/RightContent';
import { LinkOutlined } from '@ant-design/icons';
import type { Settings as LayoutSettings, MenuDataItem } from '@ant-design/pro-components';
import { SettingDrawer } from '@ant-design/pro-components';
import type { RunTimeLayoutConfig } from '@umijs/max';
import { history, Link } from '@umijs/max';
import defaultSettings from '../config/defaultSettings';
import { errorConfig } from './requestErrorConfig';
import React from 'react';
import { AvatarDropdown, AvatarName } from './components/RightContent/AvatarDropdown';
import { getLocalizationManageResources } from './services/Wheel/LocalizationManage';
import { getPermissionManage } from './services/Wheel/PermissionManage';
import { addLocale, getLocale, } from 'umi';
import enUS from 'antd/es/locale/en_US';
import { Locale } from 'antd/es/locale';
import { getCurrentMenu, getCurrentUser } from './services/Wheel/Current';
import * as allIcons from '@ant-design/icons';

const isDev = process.env.NODE_ENV === 'development';
const loginPath = '/user/login';

/**
 * @see  https://umijs.org/zh-CN/plugins/plugin-initial-state
 * */
export async function getInitialState(): Promise<{
  settings?: Partial<LayoutSettings>;
  currentUser?: API.ICurrentUser;
  loading?: boolean;
  localizationResources?: Record<string, any>;
  permissions?: API.GetAllPermissionDto[];
  fetchUserInfo?: () => Promise<API.ICurrentUser | undefined>;
  featchLocalizationManageResources?: () => Promise<Record<string, any> | undefined>;
  featchPermissions?: () => Promise<API.GetAllPermissionDto[] | undefined>;
  fetchMenuData: () => Promise<API.AntdMenuDto[]>;
}> {
  const fetchUserInfo = async () => {
    try {
      const msg = await getCurrentUser()
      return msg.data;
    } catch (error) {
      history.push(loginPath);
    }
    return undefined;
  };

  const featchLocalizationManageResources = async () => {
    try{
      const resources = await getLocalizationManageResources()
      return resources.data
    }catch(error){
      return undefined
    }
  }
  const featchPermissions = async () => {
    try{
      const resources = await getPermissionManage()
      return resources.data
    }catch(error){
      return undefined
    }
  }

  const fetchMenuData = async () => {
    try{
      const menus = await getCurrentMenu()
      return menus.data
    }catch(error){
      return []
    }
  }

  const localizationResources = await featchLocalizationManageResources();

  let locale = getLocale() as string;

  let antdLocale = {} as Locale

  if(locale === 'en-US'){
    locale = 'en'
    antdLocale = enUS
  }
  locale = getLocale() as string;
  addLocale(
    locale,
    {...localizationResources},
    {
      momentLocale: antdLocale.locale,
      antd: antdLocale,
    }
  );

  // 如果不是登录页面，执行
  const { location } = history;
  if (location.pathname !== loginPath) {
    const currentUser = await fetchUserInfo();
    const permissions = await featchPermissions();
    return {
      fetchUserInfo,
      currentUser,
      localizationResources,
      permissions,
      featchPermissions,
      featchLocalizationManageResources,
      fetchMenuData,
      settings: defaultSettings as Partial<LayoutSettings>,
    };
  }
  return {
    fetchUserInfo,
    localizationResources,
    featchLocalizationManageResources,
    settings: defaultSettings as Partial<LayoutSettings>,
  };
}

// ProLayout 支持的api https://procomponents.ant.design/components/layout
export const layout: RunTimeLayoutConfig = ({ initialState, setInitialState }) => {
  return {
    menu: {
      // 每当 initialState?.currentUser?.userid 发生修改时重新执行 request
      params: {
        userId: initialState?.currentUser,
      },
      request: async (params, defaultMenuData) => {
        const menuData = await initialState?.fetchMenuData() as MenuDataItem[];

        // FIX从接口获取菜单时icon为string类型
        const fixMenuItemIcon = (menus: MenuDataItem[], iconType = 'Outlined'): MenuDataItem[] => {
          menus.forEach((item) => {
            const { icon, children } = item;
            if (typeof icon === 'string') {
              let fixIconName = icon.slice(0, 1).toLocaleUpperCase() + icon.slice(1) + iconType;
              item.icon = React.createElement(allIcons[fixIconName] || allIcons[icon]);
            }
            if(children && children.length > 0 )
              item.children = fixMenuItemIcon(children)
          });
          return menus;
        }
        fixMenuItemIcon(menuData)
        return menuData;
      },
    },
    actionsRender: () => [<Question key="doc" />, <SelectLang key="SelectLang" />],
    avatarProps: {
      src: initialState?.currentUser?.avatar,
      title: <AvatarName />,
      render: (_, avatarChildren) => {
        return <AvatarDropdown>{avatarChildren}</AvatarDropdown>;
      },
    },
    waterMarkProps: {
      content: initialState?.currentUser?.userName,
    },
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history;
      // 如果没有登录，重定向到 login
      if (!initialState?.currentUser?.isAuthenticated && location.pathname !== loginPath) {
        history.push(loginPath);
      }
    },
    layoutBgImgList: [
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/D2LWSqNny4sAAAAAAAAAAAAAFl94AQBr',
        left: 85,
        bottom: 100,
        height: '303px',
      },
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/C2TWRpJpiC0AAAAAAAAAAAAAFl94AQBr',
        bottom: -68,
        right: -45,
        height: '303px',
      },
      {
        src: 'https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/F6vSTbj8KpYAAAAAAAAAAAAAFl94AQBr',
        bottom: 0,
        left: 0,
        width: '331px',
      },
    ],
    links: isDev
      ? [
          <Link key="openapi" to="/umi/plugin/openapi" target="_blank">
            <LinkOutlined />
            <span>OpenAPI 文档</span>
          </Link>,
        ]
      : [],
    menuHeaderRender: undefined,
    // 自定义 403 页面
    // unAccessible: <div>unAccessible</div>,
    // 增加一个 loading 的状态
    childrenRender: (children) => {
      // if (initialState?.loading) return <PageLoading />;
      return (
        <>
          {children}
          <SettingDrawer
            disableUrlParams
            enableDarkTheme
            settings={initialState?.settings}
            onSettingChange={(settings) => {
              setInitialState((preInitialState) => ({
                ...preInitialState,
                settings,
              }));
            }}
          />
        </>
      );
    },
    ...initialState?.settings,
  };
};

/**
 * @name request 配置，可以配置错误处理
 * 它基于 axios 和 ahooks 的 useRequest 提供了一套统一的网络请求和错误处理方案。
 * @doc https://umijs.org/docs/max/request#配置
 */
export const request = {
  ...errorConfig,
};
