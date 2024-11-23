import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../../models/user';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loading: boolean = false;
  loginForm: FormGroup;

  constructor(private fb: FormBuilder,
    private authService: AuthService, 
    private router: Router,
    private snackBar: MatSnackBar) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.loading = true;
      const user: User = this.loginForm.value;
      this.authService.login(user.email, user.password).subscribe({
        next: () => {
          this.loading = false;
          this.openSnackBar('Login successful.', 'Close');
          this.router.navigate(['/profile']);
        },
        error: (err) => {
          this.loading = false;
          let  errorMessage = err || 'Login failed';
          this.openSnackBar(errorMessage, 'Close');
        }
      });
    } else {
      this.openSnackBar('Please fill out the form correctly.', 'Close');
    }
  }

  register() {
    this.router.navigate(['/register']);
  }

  
  openSnackBar(message: string, action: string): void {
    this.snackBar.open(message, action, {
      duration: 3000, // Duration in milliseconds
      horizontalPosition: 'center', // Position of the toast
      verticalPosition: 'top', // Position of the toast
    });
  }
}
