import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";

export abstract class BaseHttpService {
  private readonly _apiPath: string = environment.apiPath;

  protected constructor(private readonly _httpClient: HttpClient, private readonly _modulePath: string) { }

  protected get<TResult>(path: string, params: { [header: string]: any } | undefined = undefined): Observable<TResult> {
    return this._httpClient.get<TResult>(`${this._apiPath}/${this._modulePath}/${path}`, { params });
  }

  protected post<TResult, TPayload>(path: string, body: TPayload): Observable<TResult> {
    return this._httpClient.post<TResult>(`${this._apiPath}/${this._modulePath}/${path}`, body);
  }

  protected put<TResult, TPayload>(path: string, body: TPayload): Observable<TResult> {
    return this._httpClient.put<TResult>(`${this._apiPath}/${this._modulePath}/${path}`, body);
  }

  protected delete<TResult>(path: string): Observable<TResult> {
    return this._httpClient.delete<TResult>(`${this._apiPath}/${this._modulePath}/${path}`);
  }
}
