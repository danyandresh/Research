import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NetworkSizeComponent } from './network-size/network-size.component';
import { LoginComponent } from './login/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { MatInputModule, MatTableModule, MatPaginatorModule, MatSortModule } from '@angular/material';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { DomSanitizer } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { MatListModule } from '@angular/material/list';
import { UsersessionService } from './usersession.service';
import { PointsComponent } from './points/points.component';
import { HomeComponent } from './home/home.component';
import { ProductsComponent } from './products/products.component';
import { InvestmentsComponent } from './investments/investments.component';
import { QaComponent } from './qa/qa.component'

@NgModule({
  declarations: [
    AppComponent,
    NetworkSizeComponent,
    LoginComponent,
    PointsComponent,
    HomeComponent,
    ProductsComponent,
    InvestmentsComponent,
    QaComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatCardModule,
    HttpClientModule,
    MatListModule,
    MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
