export interface User {
  userFullName?;
  id?;
  role?;
  email?;
  token?;
  createdDate?;
  password?;
  confirmPassword?;
  currentPassword?;
  newPassword?;
  confirmNewPassword?;
}

export enum Role {
  Admin = 'Admin',
  User = 'User'
}

export interface Token {
  nameid?;
  role?;
  unique_name?;
  exp?;
  sub?;
  iat?;
}
