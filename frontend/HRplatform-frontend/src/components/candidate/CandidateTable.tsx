import type { Candidate } from "../../models/Candidate";
import CandidateRow from './CandidateRow';

interface Props {
    candidates: Candidate[];
    onDelete: (id: string) => void;
    onEdit: (candidate: Candidate) => void;
    onAddSkill: (candidateId: string, skillId: string) => void;
    onRemoveSkill: (candidateId: string, skillId: string) => void; 
}

const CandidateTable = ({ candidates, onDelete, onEdit, onAddSkill, onRemoveSkill }: Props) => {
    return (
        <table cellPadding={10} style={{ borderCollapse: 'collapse', width: '100%', textAlign: 'left', 
        borderRadius: '4px', overflow: 'hidden', tableLayout: 'auto' }}>
            <thead style={{ backgroundColor: '#c3e1ff' }}>
                <tr>
                    <th>ID</th>
                    <th>Full Name</th>
                    <th>Email</th>
                    <th>Date of Birth</th>
                    <th>Contact Number</th>
                    <th>Skills</th>
                    <th style={{ width: '150px', textAlign: 'center' }}>Actions</th>
                </tr>
            </thead>
            <tbody>
                {candidates.length > 0 ? (
                    candidates.map(candidate => (
                        <CandidateRow 
                            key={candidate.id} 
                            candidate={candidate} 
                            onDelete={onDelete} 
                            onEdit={onEdit}
                            onAddSkill={onAddSkill}
                            onRemoveSkill={onRemoveSkill}
                        />
                    ))
                ) : (
                    <tr>
                        <td colSpan={5} style={{ textAlign: 'center', padding: '20px' }}>
                            No candidates found.
                        </td>
                    </tr>
                )}
            </tbody>
        </table>
    );
};

export default CandidateTable;