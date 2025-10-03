package AccountAPI.Data;

import AccountAPI.Enums.GenderEnum;

public class User {
	public final Guid _id;
	public final GenderEnum _gender;
	public final String _avata;
	public final String _bio;
	public final DateTime _dateOfBirth;
	public final Guid _userId;
	public final String _fullName;
	public final String _phone;
	public final String _province;
	public final String _commune;
	public final String _detailAddress;
	public GenderEnum _unnamed_GenderEnum_;

	public Guid getId() {
		return this._id;
	}

	public GenderEnum getGender() {
		return this._gender;
	}

	public String getAvata() {
		return this._avata;
	}

	public String getBio() {
		return this._bio;
	}

	public DateTime getDateOfBirth() {
		return this._dateOfBirth;
	}

	public Guid getUserId() {
		return this._userId;
	}

	public String getFullName() {
		return this._fullName;
	}

	public String getPhone() {
		return this._phone;
	}

	public String getProvince() {
		return this._province;
	}

	public String getCommune() {
		return this._commune;
	}

	public String getDetailAddress() {
		return this._detailAddress;
	}
}