package AccountAPI.DTO.Response;

public class ApiResponse<T> {
	public final boolean _success;
	public final String _message;
	public final T _data;

	public ApiResponse<T> Ok(T aData, String aMessage) {
		throw new UnsupportedOperationException();
	}

	public ApiResponse<T> Fail(String aMessage) {
		throw new UnsupportedOperationException();
	}

	public boolean isSuccess() {
		return this._success;
	}

	public String getMessage() {
		return this._message;
	}

	public T getData() {
		return this._data;
	}
}