import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../../models/user';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  loading: boolean = false;
  registerForm: FormGroup;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  register() {
    if (this.registerForm.valid) {
      this.loading = true;
      const user: User = this.registerForm.value;
      this.authService.register(user.username, user.email, user.password).subscribe({
        next: () => {
          this.loading = false;
          this.openSnackBar('Registration successful.', 'Close');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          this.loading = false;
          let  errorMessage = err || 'Registration failed';
          this.openSnackBar(errorMessage, 'Close');
        }
      });
    } else {
      this.openSnackBar('Please fill out the form correctly.', 'Close');
    }
  }

  login() {
    this.router.navigate(['/login']);
  }

  
  openSnackBar(message: string, action: string): void {
    this.snackBar.open(message, action, {
      duration: 3000, // Duration in milliseconds
      horizontalPosition: 'center', // Position of the toast
      verticalPosition: 'top', // Position of the toast
    });
  }
}
