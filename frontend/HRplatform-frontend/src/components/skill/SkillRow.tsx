import type { Skill } from "../../models/Skill";

interface Props {
    skill: Skill;
    onDelete: (id: string) => void;
}

const SkillRow = ({ skill, onDelete }: Props) => {
    return (
        <tr>
            <td style={{ color: '#666' }}>
                {skill.id}
            </td>
            <td><strong>{skill.name}</strong></td>
            <td style={{ textAlign: 'center' }}>
                <button 
                    onClick={() => onDelete(skill.id)}
                    style={{ 
                        padding: '4px 8px',
                        cursor: 'pointer',
                        fontSize: '15px',
                        border: '1px solid white',
                        borderRadius: '4px',
                        background: '#dc3545',
                        color: 'white'
                    }}
                >
                    Delete
                </button>
            </td>
        </tr>
    );
};

export default SkillRow;