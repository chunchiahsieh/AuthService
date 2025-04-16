import React, { useState } from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Paper from '@mui/material/Paper';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import {useLogin} from "../../models/AuthModel";
import { useNavigate } from 'react-router-dom';


const theme = createTheme();
export function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const mutation = useLogin();
  const navigate = useNavigate(); // 用於登入成功後跳轉頁面

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      mutation.mutate( { email, password }
        ,{
          onSuccess: (data) => {
            window.dispatchEvent(new Event("auth-success"));
            console.log("登入成功", data);
          },
          onError: (error: any) => {
            window.dispatchEvent(new Event("auth-failed"));
            console.error("登入失敗:", error);
          },
        }
      );
    } catch (err) {
      console.error("發生異常錯誤:", err);
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center', // 水平居中
          alignItems: 'center', // 垂直居中
          height: '100vh', // 高度設為100%視窗高度
          width: '100%', // 保證寬度覆蓋整個視窗
        }}
      >
        <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 400 }}>
          <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Sign in
          </Typography>
          <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email"
              name="email"
              autoComplete="email"
              autoFocus
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email"
              value={email}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Password"
              type="password"
              id="password"
              autoComplete="current-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <FormControlLabel
              control={<Checkbox value="remember" color="primary" />}
              label="Remember me"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Sign In
            </Button>
            {mutation.isError && (
              <Typography color="error" sx={{ fontSize: "14px", textAlign: "center" }}>
                登入失敗，請檢查帳號密碼
              </Typography>
            )}
          </Box>
        </Paper>
      </Box>
    </ThemeProvider>
  );
}

export default Login;
