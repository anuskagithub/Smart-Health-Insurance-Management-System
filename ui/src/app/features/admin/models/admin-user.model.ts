export interface AdminUser {
  id: string;
  userName: string;
  email: string;
  isApproved: boolean;
  registeredOn: string;
  roles: string[];
}

