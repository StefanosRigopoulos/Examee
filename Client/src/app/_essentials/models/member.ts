import { Exam } from "./exam";

export interface Member {
    id: number;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
    created: Date;
    photoURL: string;
    exams: Exam[];
}