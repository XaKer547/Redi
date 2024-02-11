using Geocoding;
using Geocoding.Google;

namespace Redi.Application.Helpers
{
    public static class AddressExtensions
    {
        public static async Task<Location?> GetLocationAsync(string address)
        {
            IGeocoder geocoder = new GoogleGeocoder();

            var addresses = await geocoder.GeocodeAsync(address);

            return addresses.FirstOrDefault()?.Coordinates;
        }
    }
}
