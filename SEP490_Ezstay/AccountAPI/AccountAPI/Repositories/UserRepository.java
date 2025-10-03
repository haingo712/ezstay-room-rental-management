package AccountAPI.Repositories;

import AccountAPI.Data.User;
import AccountAPI.Repositories.Interfaces.IUserRepository;

public class UserRepository implements IUserRepository {
	private IMongoCollection<User> _collection;

	public UserRepository(IMongoDatabase aDatabase) {
		throw new UnsupportedOperationException();
	}

	public async_Task CreateUserAsync(User aUser) {
		throw new UnsupportedOperationException();
	}

	public async_Task<?User> GetByUserIdAsync(Guid aUserId) {
		throw new UnsupportedOperationException();
	}

	public async_Task UpdateAsync(User aUser) {
		throw new UnsupportedOperationException();
	}
}