using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWhere.Models
{
    public class OpenWeatherMapAPIRequestParameters
    {
        public ResponseMode Mode { get; set; }//json by default, or xml
        public string Q { get; set; }//state city, example: London, Moscow, имя города может быть указано на РУССКОМ ЯЗЫКЕ
        public int? Cnt { get; set; }//number of timestamps
        public required string AppId { get; set; }//API-key
        public double? Lat { get; set; }//latitude of location(instead of Q)
        public double? Lon { get; set; }//longitude of location(instead of Q)
        public ResponseLanguage Lang { get; set; }//language of response
        public int? ZipCode { get; set; }//Code(created in USA) of Country==Postal Code(In Other Countries) (instead of Q and Lat/Lon)
        public ResponseLanguage CountryCode { get; set; }// used with zip code: zip={zip-code},{country-code-in-low-case}
        public int? Id { get; set; }//CityId used instead of ZipCode/LatLon/Q
        public ResponseUnit Units { get; set; }//Format Of Measurements
    }
    public enum ResponseMode { Json, Xml }
    public enum ResponseUnit { Metric, Standard, Imperial }//Metric - Celsius, Standard - Kelvin, Imperial - Farenheit
    public enum ResponseLanguage
    {
        ru,
        sq,
        af,
        ar,
        az,
        eu,
        be,
        bg,
        ca,
        zh_cn,
        zh_tw,
        hr,
        cz,
        da,
        nl,
        en,
        fi,
        fr,
        gl,
        de,
        el,
        he,
        hi,
        hu,
        Is,
        id,
        it,
        ja,
        kr,
        ku,
        la,
        lt,
        mk,
        no,
        fa,
        pl,
        pt,
        pt_br,
        ro,
        sr,
        sk,
        sl,
        sp,
        sv,
        th,
        tr,
        ua,
        vi,
        zu
    }
}
