//var UserProfileJSON: IUserProfile;
function StartEMIP() {
    function ResendAllData() {
        HTTPSet("WebAPI/ResendAll", "{}", (HTTPStatus, ResponseText) => {
            //UserProfileJSON     = ParseJSON_LD<IUserProfile>(ResponseText);
            //Username.value      = UserProfileJSON.name;
            //EMailAddress.value  = UserProfileJSON.email;
            //UpdateI18N(Description, UserProfileJSON.description);
        }, (HTTPStatus, StatusText, ResponseText) => {
        });
    }
    var ResendAllDataButton = document.getElementById('ResendAllData');
    ResendAllDataButton.onclick = function (ev) {
        ResendAllData();
    };
}
//# sourceMappingURL=index.js.map