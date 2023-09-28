declare namespace API {
  type AccessTokenResponse = {
    tokenType?: string;
    accessToken?: string;
    expiresIn?: number;
    refreshToken?: string;
  };

  type AntdMenuDto = {
    /** 名称 */
    name?: string;
    /** 菜单路径 */
    path?: string;
    component?: string;
    /** 图标 */
    icon?: string;
    access?: string;
    /** 子菜单 */
    children?: AntdMenuDto[];
  };

  type confirmEmailParams = {
    userId?: string;
    code?: string;
    changedEmail?: string;
  };

  type CreateLocalizationCultureDto = {
    name?: string;
  };

  type CreateLocalizationResourceDto = {
    key?: string;
    value?: string;
    cultureId?: number;
  };

  type CreateOrUpdateMenuDto = {
    /** 名称 */
    name?: string;
    /** 显示名称 */
    displayName?: string;
    menuType?: MenuType;
    /** 菜单路径 */
    path?: string;
    /** 图标 */
    icon?: string;
    /** 权限名称 */
    permission?: string;
    /** 排序 */
    sort?: number;
    /** 上级菜单Id */
    parentId?: string;
  };

  type CreateRoleDto = {
    name?: string;
    roleType?: RoleType;
  };

  type CreateUserDto = {
    userName?: string;
    passowrd?: string;
    email?: string;
    phoneNumber?: string;
    roles?: string[];
  };

  type deleteLocalizationManageCultureIdParams = {
    id: number;
  };

  type deleteLocalizationManageResourceIdParams = {
    id: number;
  };

  type deleteMenuIdParams = {
    id: string;
  };

  type deleteRoleManageParams = {
    roleName?: string;
  };

  type ForgotPasswordRequest = {
    email?: string;
  };

  type GetAllPermissionDto = {
    group?: string;
    summary?: string;
    permissions?: PermissionDto[];
  };

  type getLocalizationManageCultureIdParams = {
    id: number;
  };

  type getLocalizationManageCultureParams = {
    PageIndex?: number;
    PageSize?: number;
    OrderBy?: string;
  };

  type getMenuIdParams = {
    id: string;
  };

  type getMenuRoleRoleIdParams = {
    roleId: string;
  };

  type getPermissionManageRoleParams = {
    role: string;
  };

  type getRoleManagePageParams = {
    PageIndex?: number;
    PageSize?: number;
    OrderBy?: string;
  };

  type getSupportDataEnumsTypeParams = {
    type: string;
  };

  type getUserManageParams = {
    UserName?: string;
    Email?: string;
    EmailConfirmed?: boolean;
    PhoneNumber?: string;
    PhoneNumberConfirmed?: boolean;
    CreationTimeFrom?: string;
    CreationTimeTo?: string;
    PageIndex?: number;
    PageSize?: number;
    OrderBy?: string;
  };

  type HttpValidationProblemDetails = {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    errors?: Record<string, any>;
  };

  type ICurrentUser = {
    isAuthenticated?: boolean;
    id?: string;
    userName?: string;
    roles?: string[];
  };

  type InfoRequest = {
    newEmail?: string;
    newPassword?: string;
    oldPassword?: string;
  };

  type InfoResponse = {
    email?: string;
    isEmailConfirmed?: boolean;
    claims?: Record<string, any>;
  };

  type LabelValueDto = {
    label?: string;
    value?: any;
  };

  type LocalizationCultureDto = {
    id?: number;
    name?: string;
    resources?: LocalizationResourceDto[];
  };

  type LocalizationResourceDto = {
    id?: number;
    key?: string;
    value?: string;
    cultureId?: number;
  };

  type LoginRequest = {
    email?: string;
    password?: string;
    twoFactorCode?: string;
    twoFactorRecoveryCode?: string;
  };

  type MenuDto = {
    /** Id */
    id?: string;
    /** 名称 */
    name?: string;
    /** 显示名称 */
    displayName?: string;
    menuType?: MenuType;
    /** 菜单路径 */
    path?: string;
    /** 图标 */
    icon?: string;
    /** 权限名称 */
    permission?: string;
    /** 排序 */
    sort?: number;
    /** 上级菜单Id */
    parentId?: string;
    /** 子菜单 */
    children?: MenuDto[];
  };

  type MenuType = 0 | 1 | 2;

  type PageLocalizationCultureDto = {
    code?: string;
    message?: string;
    data?: LocalizationCultureDto[];
    total?: number;
  };

  type PageRoleDto = {
    code?: string;
    message?: string;
    data?: RoleDto[];
    total?: number;
  };

  type PageUserDto = {
    code?: string;
    message?: string;
    data?: UserDto[];
    total?: number;
  };

  type PermissionDto = {
    name?: string;
    summary?: string;
    isGranted?: boolean;
  };

  type postIdentityLoginParams = {
    useCookies?: boolean;
    useSessionCookies?: boolean;
  };

  type putMenuIdParams = {
    id: string;
  };

  type putMenuRoleRoleIdParams = {
    roleId: string;
  };

  type R = {
    code?: string;
    message?: string;
  };

  type RDictionaryStringString = {
    code?: string;
    message?: string;
    data?: Record<string, any>;
  };

  type RefreshRequest = {
    refreshToken?: string;
  };

  type RegisterRequest = {
    email?: string;
    password?: string;
  };

  type ResendEmailRequest = {
    email?: string;
  };

  type ResetPasswordRequest = {
    email?: string;
    resetCode?: string;
    newPassword?: string;
  };

  type RICurrentUser = {
    code?: string;
    message?: string;
    data?: ICurrentUser;
  };

  type RListAntdMenuDto = {
    code?: string;
    message?: string;
    data?: AntdMenuDto[];
  };

  type RListGetAllPermissionDto = {
    code?: string;
    message?: string;
    data?: GetAllPermissionDto[];
  };

  type RListLabelValueDto = {
    code?: string;
    message?: string;
    data?: LabelValueDto[];
  };

  type RListMenuDto = {
    code?: string;
    message?: string;
    data?: MenuDto[];
  };

  type RLocalizationCultureDto = {
    code?: string;
    message?: string;
    data?: LocalizationCultureDto;
  };

  type RLocalizationResourceDto = {
    code?: string;
    message?: string;
    data?: LocalizationResourceDto;
  };

  type RMenuDto = {
    code?: string;
    message?: string;
    data?: MenuDto;
  };

  type RoleDto = {
    id?: string;
    name?: string;
    roleType?: RoleType;
  };

  type RoleType = 0 | 1;

  type TwoFactorRequest = {
    enable?: boolean;
    twoFactorCode?: string;
    resetSharedKey?: boolean;
    resetRecoveryCodes?: boolean;
    forgetMachine?: boolean;
  };

  type TwoFactorResponse = {
    sharedKey?: string;
    recoveryCodesLeft?: number;
    recoveryCodes?: string[];
    isTwoFactorEnabled?: boolean;
    isMachineRemembered?: boolean;
  };

  type UpdateLocalizationResourceDto = {
    id?: number;
    key?: string;
    value?: string;
  };

  type UpdatePermissionDto = {
    type?: string;
    value?: string;
    permissions?: string[];
  };

  type UpdateRoleMenuDto = {
    menuIds?: string[];
  };

  type UserDto = {
    creationTime?: string;
    userName?: string;
    email?: string;
    emailConfirmed?: boolean;
    phoneNumber?: string;
    phoneNumberConfirmed?: boolean;
  };
}
