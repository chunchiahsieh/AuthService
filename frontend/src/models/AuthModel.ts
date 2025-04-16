import { useMutation  } from "@tanstack/react-query";
import { LoginService } from "../api"; // 用相對路徑 import
import { LoginRequest, LoginResponse } from "../api";

export const useLogin = () => {
    return useMutation<LoginResponse, Error, LoginRequest>({
        mutationFn: LoginService.postLoginLogin
    });
};
