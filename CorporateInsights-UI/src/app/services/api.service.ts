import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InsightArticle } from '../models/article.model';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    private apiUrl = 'http://localhost:5253/api/Insights';

    constructor(private http: HttpClient) { }

    getArticles(): Observable<InsightArticle[]> {
        return this.http.get<InsightArticle[]>(this.apiUrl);
    }
}