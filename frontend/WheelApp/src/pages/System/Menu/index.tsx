import { DownOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns } from '@ant-design/pro-components';
import { ProTable } from '@ant-design/pro-components';
import { Button, Tag } from 'antd';
import { L } from '@/Utilities/Localization';
import { getMenu } from '@/services/Wheel/Menu';
import menu from '@/locales/bn-BD/menu';
import { useRef } from 'react';

const columns: ProColumns<API.MenuDto>[] = [
  {
    title: 'Id',
    width: 120,
    dataIndex: 'id',
    hideInTable: true,
    valueType: 'text',
  },
  {
    title: L('name'),
    width: 120,
    dataIndex: 'name',
    valueType: 'text',
  },
  {
    title: L('displayName'),
    width: 120,
    dataIndex: 'displayName',
  },
  {
    title: L('menuType'),
    width: 120,
    dataIndex: 'menuType',
    valueEnum: {
      0: { text: L('MenuType.Menu', '菜单') },
      1: { text: L('MenuType.Page', '页面') },
      2: { text: L('MenuType.Button', '按钮') }
    }
  },
  {
    title: L('path'),
    width: 120,
    dataIndex: 'path',
  },
  {
    title: L('sort'),
    width: 120,
    dataIndex: 'sort',
  }
];

const Menu: React.FC = () => {

  const ref = useRef<ActionType>();
  const reload = () => {
    ref.current?.reload();
  }
  return (
    <ProTable<API.MenuDto>
      columns={columns}
      request={async (params, sorter, filter) => {
        // 表单搜索项会从 params 传入，传递给后端接口。
        console.log(params, sorter, filter);
        const menuData = await getMenu();
        return Promise.resolve({
          data: menuData.data,
          success: true,
        });
      }}
      rowKey="key"
      pagination={false}
      search={false}
      dateFormatter="string"
      headerTitle={L('menu.SystemManage.MenuManage')}
      options={false}
      actionRef={ref}
      toolBarRender={() => [
        <Button key="primary" type="primary" onClick={reload}>
          {L('refresh')}
        </Button>,
      ]}
    />
  );
};

export default Menu;
