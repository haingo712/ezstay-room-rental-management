package AccountAPI;

public class WeatherForecast {
	public final DateOnly _date;
	public final int _temperatureC;
	public final String _summary;
	public int _temperatureF = > 32 + (int)(TemperatureC / 0.5556);

	public DateOnly getDate() {
		return this._date;
	}

	public int getTemperatureC() {
		return this._temperatureC;
	}

	public String getSummary() {
		return this._summary;
	}
}