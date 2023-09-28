import { EditOutlined, PlusOutlined, RedoOutlined } from '@ant-design/icons';
import type { ActionType, ProColumns, ProFormColumnsType, ProFormLayoutType } from '@ant-design/pro-components';
import { ProTable, BetaSchemaForm, ProFormSelect } from '@ant-design/pro-components';
import { Button, Tag } from 'antd';
import { L } from '@/Utilities/Localization';
import { getMenu, postMenu,deleteMenuId, putMenuId } from '@/services/Wheel/Menu';
import { useRef, useState } from 'react';
let parents : any = []
const columns: ProColumns<API.MenuDto>[] = [
  {
    title: 'Id',
    width: 120,
    dataIndex: 'id',
    hideInTable: true,
    hideInForm: true,
    valueType: 'text',
  },
  {
    title: L('parentId'),
    width: 240,
    dataIndex: 'parentId',
    hideInTable: true,
    request: async () => {
      return parents
    }
  },
  {
    title: L('name'),
    width: 240,
    dataIndex: 'name',
    valueType: 'text',
  },
  {
    title: L('displayName'),
    width: 240,
    dataIndex: 'displayName',
  },
  {
    title: L('menuType'),
    width: 240,
    dataIndex: 'menuType',
    valueType: "select",
    valueEnum: {
      0: { text: L('MenuType.Menu', '菜单') },
      1: { text: L('MenuType.Page', '页面') },
      2: { text: L('MenuType.Button', '按钮') }
    }
  },
  {
    title: L('path'),
    width: 240,
    dataIndex: 'path',
  },
  {
    title: 'icon',
    width: 240,
    dataIndex: 'icon',
  },
  {
    title: L('sort'),
    width: 120,
    dataIndex: 'sort',
    valueType: 'digit'
  },
  {
    title: L('operate'),
    width: 120,
    valueType: 'option',
    key: 'option',
    render: (text, record, _, action) => [
      <a
        key="editable"
        onClick={() => {
          action?.startEditable?.(record.id);
        }}
      >
        <EditOutlined />{L('edit')}
      </a>
    ],
  }
];

const Menu: React.FC = () => {
  const [layoutType] = useState<ProFormLayoutType>('ModalForm');

  const ref = useRef<ActionType>();
  const reload = () => {
    ref.current?.reload();
  }
  return (
    <ProTable<API.MenuDto>
      columns={columns}
      request={async () => {
        // 表单搜索项会从 params 传入，传递给后端接口。
        const menuData = await getMenu();
        parents = menuData.data?.map((val) => {
          return { 'label': val.name, 'value': val.id } as any
        }) || []
        return Promise.resolve({
          data: menuData.data,
          success: true,
        });
      }}
      rowKey="id"
      pagination={false}
      search={false}
      dateFormatter="string"
      headerTitle={L('menu.SystemManage.MenuManage')}
      options={false}
      editable={{
        onDelete: async (rowKey) =>{
          await deleteMenuId({id: rowKey as string})
          reload()
        },
        onSave: async (rowKey, data) => {
          data.children = undefined
          await putMenuId({id: rowKey as string}, data as API.CreateOrUpdateMenuDto)
          reload()
        },
      }}
      actionRef={ref}
      toolBarRender={() => [
        <BetaSchemaForm<API.MenuDto>
          key="add"
          trigger={<Button type="default"><PlusOutlined />{L('add')}</Button>}
          layoutType={layoutType}
          rowProps={{
            gutter: [16, 16],
          }}
          colProps={{
            span: 12,
          }}
          shouldUpdate={false}
          grid={true}
          submitTimeout={2000}
          onFinish={async (values) => {
            values.menuType = Number(values.menuType) as API.MenuType
            await postMenu(values as API.CreateOrUpdateMenuDto )
            reload()
            return true
          }}
          {...{
            modalProps: { destroyOnClose: true }
          }}
          columns={columns as any}
        />
        ,
        <Button key="refresh" type="primary" onClick={reload}>
          <RedoOutlined /> {L('refresh')}
        </Button>,
      ]}
    />
  );
};

export default Menu;
