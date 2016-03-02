using System.Threading.Tasks;
using CoreTweet;
using Prism.Mvvm;

namespace Geopelia.Models
{
    class UserModel : BindableBase
    {
        private readonly Tokens _tokens;

        public UserModel(Tokens tokens)
        {
            this._tokens = tokens;
        }

        /// <summary>
        /// 任意のユーザのプロフィール情報を取得する
        /// </summary>
        /// <returns></returns>
        public async Task<UserResponse> GetUserAsync(long userId)
            => await this._tokens.Users.ShowAsync(userId);
    }
}
