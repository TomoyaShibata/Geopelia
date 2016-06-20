using CoreTweet;
using Prism.Mvvm;

namespace Geopelia.Models
{
    public class UserItem : BindableBase
    {
        public UserResponse UserResponse { get; set; }
        public string       NormalizedProfileBackgroundColor => this.GetNormalizedProfileBackgroundColor() ?? "Transparent";
        public string       NormalizedProfileTextColor       => this.GetNormalizedProfileTextColor()       ?? "Transparent";

        public UserItem(UserResponse userResponse)
        {
            this.UserResponse = userResponse;
        }

        /// <summary>
        /// 16進数正規化した UserResponse.ProfileBackgroundColor を返却する
        /// </summary>
        /// <returns></returns>
        private string GetNormalizedProfileBackgroundColor() => "#" + this.UserResponse.ProfileBackgroundColor;

        /// <summary>
        /// 16進数正規化した UserResponse.ProfileTextColor を返却する
        /// </summary>
        /// <returns></returns>
        private string GetNormalizedProfileTextColor() => "#" + this.UserResponse.ProfileTextColor;
    }
}
