import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../../models/user';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  loading: boolean = false;
  isEditing: boolean = false;
  profileForm: FormGroup;
  user: User = new User();

  constructor(private fb: FormBuilder, 
    private userService: UserService, 
    private authService: AuthService, 
    private router: Router,
    private snackBar: MatSnackBar) {
    this.profileForm = this.fb.group({
      username: [this.user.username, [Validators.required, Validators.minLength(3)]],
      email: [this.user.email, [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    this.getUserProfile();
  }

  getUserProfile() {
    this.userService.getUser().subscribe((res) => {
      this.user = res.data ?? new User();
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.profileForm.patchValue({
        username: this.user.username,
        email: this.user.email
      });
    }
  }

  updateProfile(): void {
    if (this.profileForm.valid) {
      const user: User = this.profileForm.value;
      const updatedUser = { username: user.username, email: user.email };
      this.loading = true;
      this.userService.updateUser(updatedUser).subscribe({
        next: () => {
          this.loading = false;
          this.openSnackBar('User updated successfully.', 'Close');
          this.isEditing = false; // Stop editing after successful update
          this.getUserProfile(); // Refresh user data
        },
        error: (err) => {
          this.loading = false;
          let errorMessage = err || 'Update profile failed';
          this.openSnackBar(errorMessage, 'Close');
        }
      });
    } else {
      this.openSnackBar('Please fill out the form correctly.', 'Close');
    }
  }

  logout() {
    this.loading = true;
    this.authService.logout().subscribe({
      next: () => {
        this.loading = false;
        this.openSnackBar('Logout successfully.', 'Close');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.loading = false;
        let errorMessage = err || 'Update profile failed';
        this.openSnackBar(errorMessage, 'Close');
      }
    });
  }

  openSnackBar(message: string, action: string): void {
    this.snackBar.open(message, action, {
      duration: 3000, // Duration in milliseconds
      horizontalPosition: 'center', // Position of the toast
      verticalPosition: 'top', // Position of the toast
    });
  }
}
