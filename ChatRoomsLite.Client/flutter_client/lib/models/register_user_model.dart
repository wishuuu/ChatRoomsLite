class RegisterUserModel {
  String? login;
  String? password;
  String? email;

  RegisterUserModel({this.login, this.password, this.email});

  RegisterUserModel.fromJson(Map<String, dynamic> json) {
    login = json['login'];
    password = json['password'];
    email = json['email'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['login'] = login;
    data['password'] = password;
    data['email'] = email;
    return data;
  }
}