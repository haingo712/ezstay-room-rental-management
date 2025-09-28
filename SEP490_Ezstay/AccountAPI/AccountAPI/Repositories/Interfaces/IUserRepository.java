package AccountAPI.Repositories.Interfaces;

import AccountAPI.Data.User;

public interface IUserRepository {

	public Task CreateUserAsync(User aUser);

	public Task<?User> GetByUserIdAsync(Guid aUserId);

	public Task UpdateAsync(User aUser);
}