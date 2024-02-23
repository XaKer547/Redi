using Geocoding;
using Geocoding.Google;

namespace Redi.Application.Helpers
{
    public static class AddressExtensions
    {
        public static async Task<Location?> GetLocationAsync(string address)
        {
            IGeocoder geocoder = new GoogleGeocoder();

            IEnumerable<Address> addresses;

            try
            {
                addresses = await geocoder.GeocodeAsync(address).WaitAsync(TimeSpan.FromSeconds(2.5));
            }
            catch
            {
                return null;
            }

            return addresses.FirstOrDefault()?.Coordinates;
        }
    }
}
