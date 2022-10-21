import 'dart:convert';

import 'package:flutter_client/models/register_user_model.dart';
import 'package:flutter_client/settings.dart';
import 'package:http/http.dart' as http;
import '../models/auth_model.dart';

class UserApiService {
  static Future<String> login(AuthModel authModel) async {
    final response = await http.post(
      Uri.parse('${Settings.url}api/user/login'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(authModel.toJson()),
    );
    if (response.statusCode == 200) {
      return response.body;
    } else {
      throw Exception('Failed to login.');
    }
  }

  static Future register(RegisterUserModel registerUserModel) async {
    final response = await http.post(
      Uri.parse('${Settings.url}api/user/register'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(registerUserModel.toJson()),
    );
    if (response.statusCode == 200) {
      return response.body;
    } else {
      throw Exception('Failed to register.');
    }
  }
}
