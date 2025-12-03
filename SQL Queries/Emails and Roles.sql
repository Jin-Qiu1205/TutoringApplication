SELECT u.Email, u.PasswordHash, r.Name AS RoleName
FROM AspNetUserRoles ur
JOIN AspNetUsers u ON ur.UserId = u.Id
JOIN AspNetRoles r ON ur.RoleId = r.Id;