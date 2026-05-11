import type { Candidate } from "../../models/Candidate";

interface Props {
    candidate: Candidate;
    onDelete: (id: string) => void;
    onEdit: (candidate: Candidate) => void;
    onAddSkill: (candidateId: string, skillId: string) => void;
    onRemoveSkill: (candidateId: string, skillId: string) => void;
}

const CandidateRow = ({ candidate, onDelete, onEdit, onAddSkill, onRemoveSkill }: Props) => {
    return (
        <tr>
            <td style={{ color: '#666', fontSize: '13px', whiteSpace: 'nowrap' }}>
                {candidate.id}
            </td>
            <td style={{ wordBreak: 'break-word' }}><strong>{candidate.fullName}</strong></td>
            <td style={{ wordBreak: 'break-word' }}>{candidate.email}</td>
            <td style={{ wordBreak: 'break-word' }}>{candidate.dateOfBirth}</td>
            <td style={{ wordBreak: 'break-word' }}>{candidate.contactNum}</td>
            <td>
                <div style={{ display: 'flex', flexWrap: 'wrap', gap: '5px' }}>
                    {candidate.skills?.map(s => (
                        <span key={s.id} style={{ 
                            backgroundColor: '#c3e1ff', 
                            padding: '2px 8px', 
                            borderRadius: '12px', 
                            fontSize: '12px',
                            fontWeight: 'bold',
                            display: 'flex',
                            alignItems: 'center',
                            gap: '5px'
                        }}>
                            {s.name}
                            <button 
                                onClick={() => onRemoveSkill(candidate.id, s.id)}
                                style={{ 
                                    border: 'none', 
                                    background: 'none', 
                                    cursor: 'pointer', 
                                    color: '#dc3545',
                                    fontWeight: 'bold',
                                    padding: '0 2px'
                                }}
                            >
                                ×
                            </button>
                        </span>
                    ))}
                    
                    {/* it could be better :D */}
                    <button 
                        onClick={() => {
                            const skillId = prompt("Enter Skill ID to add:");
                            if(skillId) 
                                onAddSkill(candidate.id, skillId);
                        }}
                        style={{
                            padding: '2px 8px',
                            borderRadius: '12px',
                            border: '1px dashed #666',
                            fontSize: '12px',
                            cursor: 'pointer'
                        }}
                    >
                        + Add
                    </button>
                </div>
            </td>
            <td style={{ textAlign: 'center', whiteSpace: 'nowrap' }}>
                <button 
                    onClick={() => onEdit(candidate)}
                    style={{ 
                        padding: '4px 8px',
                        marginRight: '5px',
                        cursor: 'pointer',
                        background: '#ffc107',
                        border: 'none',
                        borderRadius: '4px',
                        fontWeight: 'bold'
                    }}
                >
                    Edit
                </button>
                <button 
                    onClick={() => onDelete(candidate.id)}
                    style={{ 
                        padding: '4px 8px',
                        cursor: 'pointer',
                        background: '#dc3545',
                        color: 'white',
                        border: 'none',
                        borderRadius: '4px'
                    }}
                >
                    Delete
                </button>
            </td>
        </tr>
    );
};

export default CandidateRow;