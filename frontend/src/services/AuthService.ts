import { LoginService } from "../api";
import { LoginRequest, LoginResponse } from "../models/AuthModel";

export const AuthService = {
    async login(data: LoginRequest): Promise<LoginResponse> {
        return await LoginService.postLoginLogin(data);
    },
};
