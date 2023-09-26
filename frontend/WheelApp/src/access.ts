/**
 * @see https://umijs.org/zh-CN/plugins/plugin-access
 * */
export default function access(initialState: { currentUser?: API.ICurrentUser, permissions: API.GetAllPermissionDto[] } | undefined) {
  const { currentUser, permissions } = initialState ?? {};
  let permissionMap = new Map();
  permissions?.forEach((val) => {
    return val.permissions?.forEach((val2) => {
      permissionMap.set(val2.name, val2.isGranted)
    })
  })
  const isGranted = (premission: string) => {
    return permissionMap.get(premission)
  };
  return {
    canAdmin: currentUser && currentUser.roles?.indexOf('admin') !== -1,
    isGranted
  };
}
