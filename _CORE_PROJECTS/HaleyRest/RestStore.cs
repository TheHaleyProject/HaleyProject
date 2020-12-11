using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Runtime;
using System.Runtime.CompilerServices;
using Haley.Models;
using Haley.Enums;


namespace Haley.Utils
{
    public static class RestStore
    {
        public static string jwt { get; set; } //Users responsibility to set this value.
        public static string token_prefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory_url"></param>
        /// <param name="download_list"> If null, then all files in the directory will be downloaded.</param>
        /// <param name="ignore_sha_list">If not null, such sha values will be ignored while downloading</param>
        /// <returns></returns>
        public static async Task<Dictionary<string,string>> getFilesFromGithubDirectory(string download_url, List<string> download_list = null, List<string> ignore_sha_list = null)
        {
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>(); //Dowload path , Sha value
                //Check if the directory is working
                var _result = await invoke(null, download_url, is_anonymous: true); //Whenever we send a request, it checks whether it
                if (_result.status_code != HttpStatusCode.OK) return result;
                dynamic _dir_files = JsonConvert.DeserializeObject(_result.content); //Gets all the files inside the directory

                foreach (var _file in _dir_files) //We get the list of all files in the folder. We can either choose to download single file or all the files.
                {
                    //Get file details
                    string _download_url = _file.download_url;
                    string _name = _file.name;
                    string server_sha = _file.sha;

                    //Condition 1 : Download File
                     if (download_list != null && download_list?.Count > 0)
                    {
                        if (!(download_list.Contains(_name))) continue; //Our download list doesn't contain this file
                    }

                     //Condition 2 : Ignore Sha
                     if (ignore_sha_list != null && ignore_sha_list?.Count> 0) //Very rare that two files have same sha
                    {
                        if (ignore_sha_list.Contains(server_sha)) continue; //We need to ignore this because we already have this file in our local with same sha
                    }

                    //Passed all above?  Well, go ahead and download.
                        string _downloaded_file = downloadFromWeb(_download_url, _name); //Since this is not async method, we will wait till we get the downloads
                        if (!result.ContainsKey(_downloaded_file)) result.Add(_downloaded_file, server_sha);
                 }
                return result;
            }
            catch (Exception ex) 
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="github_url">Should be a directy file and not a directory</param>
        /// <returns></returns>
        public static async Task<(string downloaded_file_sha, string downloaded_path)> getFileFromGitHub(string github_file_url)
        {
            try
            {
                string _path = null;
                string _sha = null;

                var github_response = await invoke(null, github_file_url, is_anonymous: true); //Whenever we send a request, it checks whether it
                if (github_response.status_code != HttpStatusCode.OK) return (null,null);

                    //Get content
                    dynamic _content = JsonConvert.DeserializeObject(github_response.content);
                    //Since we know that it is from github, we also know that it is supposed to have download url
                    if (_content.download_url != null)
                    {
                        _path = downloadFromWeb((string)_content.download_url, (string)_content.name);
                        _sha = _content.sha;
                    }
                return (_sha, _path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string downloadFromWeb(string _download_link, string file_name)
        {
            try
            {
                string _path = null;
                _path = Path.Combine(Path.GetTempPath() + file_name);
                if (File.Exists(_path)) File.Delete(_path); 
                Uri _download_url = new Uri((string)_download_link);
                using (var _client = new WebClient())
                {
                    _client.DownloadFile(_download_url, _path);
                }
                return _path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Models.RestResponse> invoke(object content, string url, RestMethod rest_method = RestMethod.Get, bool request_as_query = false, bool is_anonymous = false, ReturnFormat return_format = ReturnFormat.Json, bool should_serialize = true)
        {
            try
            {
                Dictionary<RestParamType, object> request_content = new Dictionary<RestParamType, object>();
                RestParamType request_method = RestParamType.RequestBody;
                if (request_as_query) request_method = RestParamType.QueryString;
                request_content.Add(request_method, content);
                return invoke(request_content, url, rest_method, is_anonymous, return_format, should_serialize).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Models.RestResponse> invoke(Dictionary<RestParamType,object> content, string url, RestMethod rest_method = RestMethod.Get, bool is_anonymous = false, ReturnFormat return_format = ReturnFormat.Json, bool should_serialize = true)
        {
            //var _url = _getBaseUrl(url);

            //INVOKE
            //var client = new RestClient(_url.base_url);
            var client = new RestClient();
            Models.RestResponse _result = new Models.RestResponse();

            Method _method = Method.GET;
            switch(rest_method)
            {
                case RestMethod.Delete:
                    _method = Method.DELETE;
                    break;
                case RestMethod.Put:
                    _method = Method.PUT;
                    break;
                case RestMethod.Post:
                    _method = Method.POST;
                    break;
            }
            //var _request = new RestRequest(_url.method_url, _method); //Each request can have only one method.
            var _request = new RestRequest(url, _method); //Each request can have only one method.

            //ADD HEADERS
            _request.AddHeader("Content-Type", @"application/json; charset=utf-8");
            if (!is_anonymous) //Need authorization to proceed. If authorization is not available, throw error
            {
                if (string.IsNullOrEmpty(jwt)) throw new ArgumentException("Authorization String not found"); //Remember you need to have a jwt value in session for processing.
                string _jwt = token_prefix ?? "";
                _jwt = _jwt + " " + jwt;
                _request.AddHeader("Authorization", _jwt); //If token prefix is null, then send in nothing.
            }

            if (content == null) throw new ArgumentNullException("Content cannot be empty"); //If we don't have anything to post, just move on

                RestParamType request_type = RestParamType.QueryString;
                foreach (var item in content)
                {
                    request_type = rest_method == RestMethod.Get ? RestParamType.QueryString : item.Key;  //For a Get method, it should only be sent as query because get methods cannot have a body.
                    _fillRequestParameters(item.Value, _request, request_type, should_serialize); //Sometimes, we just send in a dicitionary. sometimes we send in a object.
                }

            DataFormat _rest_return_format = DataFormat.Json;
            switch(return_format)
            {
                case ReturnFormat.Json:
                    _rest_return_format = DataFormat.Json;
                    break;
                case ReturnFormat.XML:
                    _rest_return_format = DataFormat.Xml;
                    break;
                case ReturnFormat.None:
                    //_rest_return_format = DataFormat.None;
                    break;
            }
            _request.RequestFormat = _rest_return_format;
            try
            {
                //Below lines are required because sometimes it results in error during async methods.
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //var _response = await client.ExecuteAsync(_request);
                var _response = client.Execute(_request);
                _result.content = _response.Content;
                _result.server_url = _response.Server;
                _result.is_success = _response.IsSuccessful;
                _result.status_code = _response.StatusCode;
                _result.contents_raw = _response.RawBytes;
                _result.content_encoding = _response.ContentEncoding;
                _result.content_length = _response.ContentLength;
                _result.error_message = _response.ErrorMessage;
                _result.exception = _response.ErrorException;
                _result.response_uri = _response.ResponseUri;
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool _fillRequestParameters(object content, RestRequest _request, RestParamType rest_param_type, bool serialize_content = false)
        {
            string _value_string = string.Empty;
            ParameterType _supply_param_type = ParameterType.QueryString;

            switch(rest_param_type)
            {
                case RestParamType.QueryString:
                    _supply_param_type = ParameterType.QueryString;
                    break;
                case RestParamType.RequestBody:
                    _supply_param_type = ParameterType.RequestBody;
                    break;
                case RestParamType.Header:
                    _supply_param_type = ParameterType.HttpHeader;
                    break;
            }

            if (rest_param_type != RestParamType.QueryString) //If parameter type is query string, then we definitely don't serialize
            {
                //You can send in a request body either serialized or not serialized
                if (serialize_content) 
                {
                    _supply_param_type = ParameterType.RequestBody;
                    _value_string = JsonConvert.SerializeObject(content);
                    _request.AddParameter("data", _value_string, _supply_param_type);
                    return true;
                }
            }

            //If it is not a serialize content, then we proceed further
            if (content.IsDictionary())
            {
                Dictionary<string, string> content_array = content as Dictionary<string, string>;
                foreach (var item in content_array)
                {
                    _request.AddParameter(item.Key, item.Value, _supply_param_type);
                }
            }
            else if (content is string)
            {
                _value_string = (string)content;
                _request.AddParameter("data", _value_string, _supply_param_type);
            }
            else //If an object , then directly get to string.
            {
                _value_string = content.ToString();
                _request.AddParameter("data", _value_string, _supply_param_type);
            }

            return true;
        }

        private static (string base_url, string method_url) _getBaseUrl(string input_url)
        {
            try
            {
                var _uri = new Uri(input_url);
                string _base = _uri.GetLeftPart(UriPartial.Authority);
                string _method = input_url.Substring(_base.Length);
                return (_base, _method);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
