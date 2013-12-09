using System;

namespace ShinobiStockChart.Utilities
{
	/// <summary>
	/// Specifies the interface for obtaining license keys
	/// </summary>
	public interface IShinobiLicenseKeyProvider
	{
		/// <summary>
		/// The license key for ShinobiCharts
		/// </summary>
		string ChartsLicenseKey { get; }

		/// <summary>
		/// The license key for ShinobiGrids
		/// </summary>
		string GridsLicenseKey { get; }
	}
}

