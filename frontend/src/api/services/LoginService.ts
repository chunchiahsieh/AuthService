/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { LoginRequest } from '../models/LoginRequest';
import type { LoginResponse } from '../models/LoginResponse';
import type { RefreshTokenRequest } from '../models/RefreshTokenRequest';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class LoginService {
    /**
     * @returns any Success
     * @throws ApiError
     */
    public static postLoginAutoRegister(): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/Login/AutoRegister',
        });
    }
    /**
     * @param requestBody
     * @returns LoginResponse Success
     * @throws ApiError
     */
    public static postLoginLogin(
        requestBody?: LoginRequest,
    ): CancelablePromise<LoginResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/Login/login',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns LoginResponse Success
     * @throws ApiError
     */
    public static postLoginRefreshToken(
        requestBody?: RefreshTokenRequest,
    ): CancelablePromise<LoginResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/Login/refresh-token',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
