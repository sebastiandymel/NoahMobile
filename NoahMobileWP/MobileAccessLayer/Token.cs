using System;

namespace Himsa.Noah.MobileAccessLayer
{
    /// <summary>
    /// Wraps token information.
    /// </summary>
    public class Token
    {
        #region Public_Properties
        /// <summary>
        /// Expiration time, if available.
        /// </summary>
        public DateTime? ExpiresUtc { get; set; }
        /// <summary>
        /// The actual access token (a string) which is used by authentication.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Is the token expired?
        /// </summary>
        public bool IsExpired
        {
            get { return ExpiresUtc.HasValue && DateTime.UtcNow > ExpiresUtc; }
        }

        #endregion

        #region Ctors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expiresUtc"></param>
        public Token(string accessToken, DateTime? expiresUtc = null)
        {
            AccessToken = accessToken;
            ExpiresUtc = expiresUtc;
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a <see cref="Token"/> from the information embedded in the re-direct url from the authentication server.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Token FromUri(Uri uri)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Token::FromUri: uri={0}", uri);
                string accessToken = null;
                TimeSpan? expiresIn = null;
                string fragment = uri.Fragment;
                if (!string.IsNullOrEmpty(fragment))
                {
                    if (fragment[0] == '#')
                        fragment = "?" + fragment.Remove(0, 1);
                    uri = new Uri("http://dummy.net" + fragment);

                    var queries = Helpers.ParseQueryString(uri);

                    if (!string.IsNullOrEmpty(queries["access_token"]))
                        accessToken = queries["access_token"];
                    if (!string.IsNullOrEmpty(queries["expires_in"]))
                        expiresIn = TimeSpan.FromSeconds(double.Parse(queries["expires_in"]));
                }
                Token token = string.IsNullOrEmpty(accessToken) ? null : new Token(accessToken, DateTime.UtcNow + expiresIn);
                if (token == null)
                    Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Token::FromUri: Token not found.");
                else
                    Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Token::FromUri: Token found.");
                return token;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Token::FromUri: {0}", e);
                throw;
            }
        }

        /// <summary>
        /// Creates a <see cref="Token"/> from the information embedded in the commadn line args.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Token FromArgs(string[] args)
        {
            try
            {
                Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Token::FromArgs: args={0}", string.Join(", ", args));
                Token token = null;
                foreach (string arg in args)
                    if (token == null && Uri.IsWellFormedUriString(arg, UriKind.RelativeOrAbsolute))
                        token = FromUri(new Uri(arg));
                if (token == null)
                    Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Token::FromArgs: Token not found.");
                else
                    Log.DebugFormat("Himsa.Noah.MobileAccessLayer.Token::FromArgs: Token found.");
                return token;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Himsa.Noah.MobileAccessLayer.Token::FromArgs: {0}", e);
                throw;
            }
        }

        #endregion
    }
}
